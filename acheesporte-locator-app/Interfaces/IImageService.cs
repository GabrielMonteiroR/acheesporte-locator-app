using acheesporte_locator_app.Dtos.ImageUploadDtos;

namespace acheesporte_locator_app.Interfaces;

public interface IImageService
{
  Task<ImageUploadResponseDto> UploadProfileImageAsync(FileResult file);
  Task<List<ImageUploadResponseDto>> UploadVenuesImageAsync(List<FileResult> files);

}
