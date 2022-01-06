using Asynchronous_TextProcessing.Classes;
using Asynchronous_TextProcessing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Asynchronous_TextProcessing.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class TaskController : Controller
{
    private readonly ILogger<TaskController> _logger;
    private readonly DBContext _context;

    public TaskController(DBContext context, ILogger<TaskController> logger)
    {
        _context = context;
        _logger = logger;
    }
    private string getUser()
    {
        return (User.Identity as ClaimsIdentity).Name;
    }


    private ActionResult resReturn(RequestT req)
    {

        switch ((RequestType)req.Type)
        {
            case RequestType.TextProcessing:
                var res = new TextProcessResponseModel()
                {
                    Id = req.Id,
                    CreateDateTime = req.CreateDateTime ?? DateTime.Now,
                    Name = req.Name,
                    Text = JsonConvert.DeserializeObject<TextProcessRequestModel>(req.RequestData).Text,
                    State = ((RequestState)req.State).ToString()
                };
                return Json(res);
        }
        return null;
    }

    [HttpPost]
    public ActionResult NewRequest([FromBody] Models.TextProcessRequestModel data)
    {
        DateTime timeNow = DateTime.Now;

        // get uiser id
        long userId = _context.UserTs.FirstOrDefault(u => u.UserName == getUser()).UserId;

        long? PermissionID = _context.PermissionTs.FirstOrDefault(p => p.Name.ToLower() == ((Permissiontype)Permissiontype.Request).ToString())?.PermissionId;
        if (PermissionID is null) return BadRequest(new BadRequestErrorModel { Error = "This no such Permission!" });

        //check USER PERMISSION
        if (!_context.UserPermissionTs.Any(up => up.UserId == userId && up.PermissionId == PermissionID))
            return Unauthorized(new BadRequestErrorModel { Error = "Permission is denied!" });


        var req = new RequestT
        {
            UserId = userId,
            Name = data.Name,
            RequestData = JsonConvert.SerializeObject(data),
            State = (byte)RequestState.Active,
            Type = (byte)RequestType.TextProcessing,
            CreateDateTime = DateTime.Now
        };

        _context.RequestTs.Add(req);
        _context.SaveChanges();

        //call engine
        PreProccess pre = new PreProccess();
        pre.ExecuteAsync(req);

        return resReturn(req);
    }

    [HttpGet]
    public ActionResult CheckRequest(int id)
    {
        var userId = _context.UserTs.FirstOrDefault(u => u.UserName == getUser()).UserId;
        long? PermissionID = _context.PermissionTs.FirstOrDefault(p => p.Name.ToLower() == ((Permissiontype)Permissiontype.Result).ToString())?.PermissionId;
        if (PermissionID is null) return BadRequest(new BadRequestErrorModel { Error = "This no such Permission!" });

        if (!_context.UserPermissionTs.Any(up => up.UserId == userId && up.PermissionId == PermissionID))
            return Unauthorized(new BadRequestErrorModel { Error = "Permission is denied!" });

        var req = _context.RequestTs.Where(a => a.Id == id).FirstOrDefault();
        if (req == null) return NotFound();

        return resReturn(req);
    }

    [HttpGet]
    public ActionResult Result(int id)
    {

        var userId = _context.UserTs.FirstOrDefault(u => u.UserName == getUser()).UserId;
        long? PermissionID = _context.PermissionTs.FirstOrDefault(p => p.Name.ToLower() == ((Permissiontype)Permissiontype.Result).ToString())?.PermissionId;
        if (PermissionID is null) return BadRequest(new BadRequestErrorModel { Error = "This no such Permission!" });

        if (!_context.UserPermissionTs.Any(up => up.UserId == userId && up.PermissionId == PermissionID))
            return Unauthorized(new BadRequestErrorModel { Error = "Permission is denied!" });

        var req = _context.RequestTs.Where(a => a.Id == id).FirstOrDefault();
        if (req == null) return BadRequest(new BadRequestErrorModel { Error = "There is no such Request!" });

        var res = _context.ResultTs.Where(b => b.Id == req.ResultId).FirstOrDefault();
        if (res == null) return BadRequest(new BadRequestErrorModel { Error = "This request currently has no results. Try again later" });

        //var fileAddress = System.IO.Path.Combine(Config.All.FilePath, (IsHost ? "H_" : "G_") + req.CreateDateTime?.ToString("yyMMddhhmmssfff") + ".csv");
        //if (!System.IO.File.Exists(fileAddress)) return BadRequest(new BadRequestErrorModel { Error = "File Not Found" });
        //byte[] bytes = System.IO.File.ReadAllBytes(fileAddress);
        //var result = new FileContentResult(bytes, "text/plain");
        //result.FileDownloadName = name + "_ActivePathList.txt";

        return Json(res.ResultData);
    }

}
