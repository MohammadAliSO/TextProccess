using Asynchronous_TextProcessing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Asynchronous_TextProcessing.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : Controller
{
    private readonly ILogger<TaskController> _logger;
    private readonly DBContext _context;

    public UserController(DBContext context, ILogger<TaskController> logger)
    {
        _context = context;
        _logger = logger;
    }
    private string getUser()
    {
        return (User.Identity as ClaimsIdentity).Name;
    }

    private UserResponseModel resReturn(UserT userT)
    {


        var res = new UserResponseModel()
        {
            UserId=userT.UserId,
            Name=userT.Name,
            Username=userT.UserName,
            CreateTime=userT.CreateTime
        };
        return res;

        return null;
    }
    [HttpGet]
    public ActionResult GetUser(long UserId)
    {
        // get user id
        long uId = _context.UserTs.FirstOrDefault(u => u.UserName == getUser()).UserId;
        long? PermissionID = _context.PermissionTs.FirstOrDefault(p => p.Name.ToLower() == ((Permissiontype)Permissiontype.GetUser).ToString())?.PermissionId;
        if (PermissionID is null) return BadRequest(new BadRequestErrorModel { Error = "This no such Permission!" });

        //check USER PERMISSION
        if (!_context.UserPermissionTs.Any(up => up.UserId == uId && up.PermissionId == PermissionID))
            return Unauthorized(new BadRequestErrorModel { Error = "Permission is denied!" });

        var user = _context.UserTs.FirstOrDefault(u => u.UserId == UserId);
        if (user is null) return NotFound(new BadRequestErrorModel { Error = "This no such User!" });

        return Json(resReturn(user));
    }
    [HttpGet]
    public ActionResult AllUsers()
    {
        // get user id
        long uId = _context.UserTs.FirstOrDefault(u => u.UserName == getUser()).UserId;
        long? PermissionID = _context.PermissionTs.FirstOrDefault(p => p.Name.ToLower() == ((Permissiontype)Permissiontype.GetUser).ToString())?.PermissionId;
        if (PermissionID is null) return BadRequest(new BadRequestErrorModel { Error = "This no such Permission!" });

        //check USER PERMISSION
        if (!_context.UserPermissionTs.Any(up => up.UserId == uId && up.PermissionId == PermissionID))
            return Unauthorized(new BadRequestErrorModel { Error = "Permission is denied!" });
        List<UserResponseModel> users = new List<UserResponseModel>();

        foreach (var user in _context.UserTs)
        {
            users.Add(resReturn(user));
        }
        
        return Json(users);
    }

    [HttpGet]
    public ActionResult RequestOfUser(long UserId)
    {
        // get user id
        long uId = _context.UserTs.FirstOrDefault(u => u.UserName == getUser()).UserId;
        long? PermissionID = _context.PermissionTs.FirstOrDefault(p => p.Name.ToLower() == ((Permissiontype)Permissiontype.RequestUser).ToString())?.PermissionId;
        if (PermissionID is null) return BadRequest(new BadRequestErrorModel { Error = "This no such Permission!" });

        //check USER PERMISSION
        if (!_context.UserPermissionTs.Any(up => up.UserId == uId && up.PermissionId == PermissionID))
            return Unauthorized(new BadRequestErrorModel { Error = "Permission is denied!" });

        var reqs = _context.RequestTs.Where(r => r.UserId == UserId).Select(r=>new { r.Id ,r.State, r.Type, r.RequestData  ,r.ResultId});

        return Json(reqs);
    }

    [HttpPost]
    public ActionResult Add([FromBody] Models.UserModel req)
    {
        // get user id
        long uId = _context.UserTs.FirstOrDefault(u => u.UserName == getUser()).UserId;
        long? PermissionID = _context.PermissionTs.FirstOrDefault(p => p.Name.ToLower() == ((Permissiontype)Permissiontype.AddUser).ToString())?.PermissionId;
        if (PermissionID is null) return BadRequest(new BadRequestErrorModel { Error = "This no such Permission!" });

        //check USER PERMISSION
        if (!_context.UserPermissionTs.Any(up => up.UserId == uId && up.PermissionId == PermissionID))
            return Unauthorized(new BadRequestErrorModel { Error = "Permission is denied!" });

        var user = new UserT()
        {
            Name = req.Name,
            UserName = req.UserName,
            Password = req.Password,
            CreateTime = DateTime.Now
        };

        _context.UserTs.Add(user);
        _context.SaveChanges();

        return Json(resReturn(user));
    }

    [HttpDelete]
    public ActionResult Delete(int UserId)
    {
        // get user id
        long uId = _context.UserTs.FirstOrDefault(u => u.UserName == getUser()).UserId;
        long? PermissionID = _context.PermissionTs.FirstOrDefault(p => p.Name.ToLower() == ((Permissiontype)Permissiontype.DeleteUser).ToString())?.PermissionId;
        if (PermissionID is null) return BadRequest(new BadRequestErrorModel { Error = "This no such Permission!" });
        //check USER PERMISSION
        if (!_context.UserPermissionTs.Any(up => up.UserId == uId && up.PermissionId == PermissionID))
            return Unauthorized(new BadRequestErrorModel { Error = "Permission is denied!" });

        var user = _context.UserTs.FirstOrDefault(u => u.UserId == UserId);
        if (user is null) return NotFound(new BadRequestErrorModel { Error = "This no such User!" });
        if(user.UserId==uId || user.UserId == 0) return BadRequest(new BadRequestErrorModel { Error = "You can not delete this user!" });
        _context.UserTs.Remove(user);
        _context.SaveChanges();

        return Ok();
    }

    //[HttpPut]
    //public ActionResult Update([FromBody] UserUpdateModel req)
    //{

    //    // get user id
    //    long userId = _context.UserTs.FirstOrDefault(u => u.UserName == getUser()).UserId;
    //    long? PermissionID = _context.PermissionTs.FirstOrDefault(p => p.Name.ToLower() == "UpdateUser")?.PermissionId;
    //    if (PermissionID is null) return BadRequest(new BadRequestErrorModel { Error = "This no such Permission!" });

    //    //check USER PERMISSION
    //    if (!_context.UserPermissionTs.Any(up => up.UserId == userId && up.PermissionId == PermissionID))
    //        return Unauthorized(new BadRequestErrorModel { Error = "Permission is denied!" });

    //    if (!_context.UserTs.Any(u => u.UserId == req.UserId)) return NotFound(new BadRequestErrorModel { Error = "This no such User!" });

    //    var user = new UserT()
    //    {
    //        UserId = req.UserId,
    //        Name = req.Name,
    //        UserName = req.UserName,
    //        Password = req.Password,
    //        CreateTime = DateTime.Now,
    //    };
    //    _context.UserTs.s(u=>u.);
    //    _context.SaveChanges();

    //    return Json(resReturn(user));
    //}

}
