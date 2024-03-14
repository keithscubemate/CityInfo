using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

    public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
    {
        this._fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ??
                                            throw new System.ArgumentNullException(
                                                nameof(fileExtensionContentTypeProvider));
    }


    [HttpGet("{fileId}")]
    public ActionResult GetFile(string fileId)
    {
        // TODO: look up file
        const string pathToFile = "getting-started-with-rest-slides.pdf";

        if (!System.IO.File.Exists(pathToFile))
        {
            return NotFound();
        }

        if (!this._fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        var bytes = System.IO.File.ReadAllBytes(pathToFile);

        return File(bytes, contentType, Path.GetFileName(pathToFile));
    }

    [HttpPost]
    public async Task<ActionResult> CreateFile(IFormFile file)
    {
        if (file.Length <= 0 || file.Length < 20971520 || file.ContentType != "application/pdf")
        {
            return BadRequest("No file or invalid file provided.");
        }

        var path = Path.Combine(
            Directory.GetCurrentDirectory(),
            $"uploaded_file_{Guid.NewGuid()}.pdf");


        await using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream);

        return Ok("Your file has been successfully uploaded");
    }
}