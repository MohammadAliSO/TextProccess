using Asynchronous_TextProcessing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Asynchronous_TextProcessing.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class PermissionController : Controller
{
    private readonly ILogger<TaskController> _logger;
    private readonly DBContext _context;

    public PermissionController(DBContext context, ILogger<TaskController> logger)
    {
        _context = context;
        _logger = logger;
    }
    private string getUser()
    {
        return (User.Identity as ClaimsIdentity).Name;
    }

    
    [HttpPost]
    public ActionResult Add(long UserId ,Permissiontype permissiontype)
    {
        // get user id
        long uId = _context.UserTs.FirstOrDefault(u => u.UserName == getUser()).UserId;
        long? PermissionID = _context.PermissionTs.FirstOrDefault(p => p.Name.ToLower() == ((Permissiontype)Permissiontype.AddPermission).ToString())?.PermissionId;
        if (PermissionID is null) return BadRequest(new BadRequestErrorModel { Error = "This no such Permission!" });

        //check USER PERMISSION
        if (!_context.UserPermissionTs.Any(up => up.UserId == uId && up.PermissionId == PermissionID))
            return Unauthorized(new BadRequestErrorModel { Error = "Permission is denied!" });

        long newPID = _context.PermissionTs.FirstOrDefault(p => p.Name.ToLower() == ((Permissiontype)permissiontype).ToString()).PermissionId;

        if (_context.UserPermissionTs.Any(up => up.UserId == UserId && up.PermissionId == newPID)) return BadRequest(new BadRequestErrorModel { Error = "This permission exist for user!" });

        _context.UserPermissionTs.Add(new UserPermissionT { UserId=UserId , PermissionId= newPID });
        _context.SaveChanges();

        return Ok();
    }
    
    [HttpDelete]
    public ActionResult Delete(long UserId, Permissiontype permissiontype)
    {
        // get user id
        long uId = _context.UserTs.FirstOrDefault(u => u.UserName == getUser()).UserId;
        long? PermissionID = _context.PermissionTs.FirstOrDefault(p => p.Name.ToLower() == ((Permissiontype)Permissiontype.DeletePermission).ToString())?.PermissionId;
        if (PermissionID is null) return BadRequest(new BadRequestErrorModel { Error = "This no such Permission!" });

        //check USER PERMISSION
        if (!_context.UserPermissionTs.Any(up => up.UserId == uId && up.PermissionId == PermissionID))
            return Unauthorized(new BadRequestErrorModel { Error = "Permission is denied!" });

        long newPID = _context.PermissionTs.FirstOrDefault(p => p.Name.ToLower() == ((Permissiontype)permissiontype).ToString()).PermissionId;

        UserPermissionT? up=_context.UserPermissionTs.FirstOrDefault(up => up.UserId == UserId && up.PermissionId == newPID);
        if (up is null) return BadRequest(new BadRequestErrorModel { Error = "This permission NOT exist for user!" });
        if (up.UserId == uId || up.UserId == 0) return BadRequest(new BadRequestErrorModel { Error = "You can not delete  persmission for this user!" });

        _context.UserPermissionTs.Remove(up);
        _context.SaveChanges();

        return Ok();
    }

    [HttpPost]
    public ActionResult GetPermissionsOfUser(long UserId)
    {
        // get user id
        long uId = _context.UserTs.FirstOrDefault(u => u.UserName == getUser()).UserId;
        long? PermissionID = _context.PermissionTs.FirstOrDefault(p => p.Name.ToLower() == ((Permissiontype)Permissiontype.AddPermission).ToString())?.PermissionId;
        if (PermissionID is null) return BadRequest(new BadRequestErrorModel { Error = "This no such Permission!" });

        //check USER PERMISSION
        if (!_context.UserPermissionTs.Any(up => up.UserId == uId && up.PermissionId == PermissionID))
            return Unauthorized(new BadRequestErrorModel { Error = "Permission is denied!" });


        var permissions = _context.UserPermissionTs.Where(up=>up.UserId==UserId).Join(_context.PermissionTs, up => up.PermissionId, p => p.PermissionId, (up, p) => new { Permission= p }).Select(a=>a.Permission.Name).ToList();
        return Json(permissions);
    }


}
