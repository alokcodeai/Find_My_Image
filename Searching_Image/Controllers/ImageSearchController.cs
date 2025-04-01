using Microsoft.AspNetCore.Mvc;
using Searching_Image.Models;
using System.Drawing;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Searching_Image.Controllers
{
   

    public class ImageSearchController : Controller
    {
        private HttpClient _httpClient;
        private readonly UnsplashService _unsplashService;

        public ImageSearchController(HttpClient httpClient, UnsplashService unsplashService)
        {
            _httpClient = httpClient;
            _unsplashService = unsplashService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var images = await _unsplashService.GetRandomImagesAsync();
            return View(images);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View("Index", new List<UnsplashImage>());
            }

            var images = await _unsplashService.SearchImagesAsync(query);
            return View("Index", images);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var image = await _unsplashService.GetImageByIdAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            return View(image);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var image = await _unsplashService.GetImageByIdAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            return View(image);
        }

        [HttpGet]
        public async Task<IActionResult> Download(string id)
        {
            var image = await _unsplashService.GetImageByIdAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            // Use the appropriate property, e.g., Regular if Full is not available
            var response = await _httpClient.GetAsync(image.Urls.Regular);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var imageStream = await response.Content.ReadAsStreamAsync();
            var contentType = response.Content.Headers.ContentType?.ToString() ?? "image/jpeg";
            var fileName = $"{id}.jpg";

            return File(imageStream, contentType, fileName);
        }

        //[HttpGet]
        //public async Task<IActionResult> SaveImage(string imageData)
        //{

        //    Random rnd = new Random();
        //    String Filename = HttpContext.Current.Server.MapPath("Shanuimg" + rnd.Next(12, 2000).ToString() + ".png");
        //    string Pic_Path = Filename;//HttpContext.Current.Server.MapPath("ShanuHTML5DRAWimg.png");    
        //    using (FileStream fs = new FileStream(Pic_Path, FileMode.Create))
        //    {
        //        using (BinaryWriter bw = new BinaryWriter(fs))
        //        {
        //            byte[] data = Convert.FromBase64String(imageData);
        //            bw.Write(data);
        //            bw.Close();
        //        }
        //    }
        //    return File(imageStream, contentType, fileName);
        //}

    }
}


