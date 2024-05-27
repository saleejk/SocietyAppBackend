﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocietyAppBackend.ModelEntity;
using SocietyAppBackend.Service.LikeService;

namespace SocietyAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        public readonly ILikeServices _likeServices;
        public LikeController(ILikeServices likeServices)
        {
            _likeServices= likeServices;
        }
        [HttpGet("LikePost")]
        public async Task<IActionResult>LikePost(int userid,int postid)
        {
            try
            {
                if (userid == null || postid == null)
                {
                    return BadRequest("invalid request");
                }
                await _likeServices.LikePost(userid, postid);
                return Ok("liked successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"something went wrong{ex.Message}");
            }
        }
        [HttpDelete("UnLikePost")]
        public async Task<IActionResult>UnLikePost(int userid,int postid)
        {

            try
            {
                if (userid == null || postid == null)
                {
                    return BadRequest("badRequest");
                }
                await _likeServices.UnLikePost(userid, postid);
                return Ok("unlikeSuccessfull");
            }
            catch (Exception ex) { return BadRequest("something went wrong");
            }
        }

    }

}
