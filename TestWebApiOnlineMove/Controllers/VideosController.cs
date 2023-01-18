using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TestWebApiOnlineMove.Context;
using TestWebApiOnlineMove.Models;

namespace TestWebApiOnlineMove.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideosController : Controller
    {
        private readonly AppDbContext _context;

        private int UserId => int.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public VideosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("getAll")]
        public async Task<List<Video>> GetVideo()
        {
            var list = await _context.Videos.ToListAsync();

            return list;
        }
        [Authorize(Roles = "User")]
        [HttpGet("getAllGenre")]
        public async Task<List<Genres>> GetGenres()
        {
            var list = await _context.Genres.ToListAsync();

            return list;
        }


        [HttpPost("addGenre")]
        public async Task<bool> CreateVideo(Genres model)
        {
            try
            {
                var saveModel = new Genres()
                {
                    Id = model.Id,
                    Genre = model.Genre,
                };
                if (!await _context.Genres.AnyAsync(x => x.Id == model.Id))
                {
                    await _context.Genres.AddAsync(saveModel);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet("{id}")]
        public async Task<Video> GetByIdVideo(int id)
        {
            var list = await _context.Videos.FirstOrDefaultAsync(x => x.Id == id);

            return list;
        }

        [HttpPost("create")]
        public async Task<bool> CreateVideo(Video model)
        {
            try
            {
                var saveModel = new Video()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    URLIMG = model.URLIMG,
                    URLVideo = model.URLVideo,
                    NumberViews = model.NumberViews,
                    Rate = model.Rate,
                    RateCount = model.RateCount,
                    Tags = model.Tags,
                };
                if (!await _context.Videos.AnyAsync(x => x.Id == model.Id))
                {
                    await _context.Videos.AddAsync(saveModel);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        [HttpDelete("delete")]
        public async Task<bool> DeleteVideo(Video model)
        {
            if (await _context.Videos.AnyAsync(x => x.Id == model.Id))
            {
                _context.Videos.Remove(model);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        [HttpDelete("delete/{id}")]
        public async Task<bool> DeleteVideo(int id)
        {
            if (await _context.Videos.AnyAsync(x => x.Id == id))
            {
                Video model = await _context.Videos.FirstOrDefaultAsync(x => x.Id == id);
                _context.Videos.Remove(model);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        [HttpPut("edit")]
        public async Task<bool> EditVideo(Video model)
        {
            try
            {
                var video = await _context.Videos.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (video != null && video != new Video())
                {
                    video.Name = (model.Name != null) ? model.Name : video.Name;
                    video.Description = model.Description;
                    video.URLIMG = model.URLIMG;
                    video.URLVideo = model.URLVideo;
                    video.NumberViews = model.NumberViews;
                    video.Rate = model.Rate;
                    video.RateCount = model.RateCount;
                
                    _context.Videos.Update(video);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch { return false; }
        }

        [HttpPut("addOneRate/{id}/{rate}")]
        public async Task<bool> AddOneRateVideo(int id, double rate)
        {
            try
            {
                var video = await _context.Videos.FirstOrDefaultAsync(x => x.Id == id);
                if (video != null && video != new Video())
                {

                    video.Rate = ((video.Rate * video.RateCount) + rate) / (video.RateCount + 1);
                    video.RateCount++;

                    _context.Videos.Update(video);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch { return false; }
        }

    }
}
