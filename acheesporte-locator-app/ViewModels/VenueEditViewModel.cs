using acheesporte_locator_app.Dtos.VenueDtos;
using acheesporte_locator_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace acheesporte_locator_app.ViewModels;

public partial class VenueEditViewModel : ObservableObject
{
    private readonly IVenueService _venueService;
    private readonly IImageInterface _imageService;

    public VenueEditViewModel(IVenueService venueService, IImageInterface imageService)
    {
        _venueService = venueService;
        _imageService = imageService;

        ImageUrls = new ObservableCollection<string>();
    }

    [ObservableProperty] private int venueId;
    [ObservableProperty] private string name;
    [ObservableProperty] private string street;
    [ObservableProperty] private string number;
    [ObservableProperty] private string complement;
    [ObservableProperty] private string neighborhood;
    [ObservableProperty] private string city;
    [ObservableProperty] private string state;
    [ObservableProperty] private string postalCode;
    [ObservableProperty] private double latitude;
    [ObservableProperty] private double longitude;
    [ObservableProperty] private string description;
    [ObservableProperty] private int capacity;
    [ObservableProperty] private string rules;
    [ObservableProperty] private int venueTypeId;
    [ObservableProperty] private ObservableCollection<string> imageUrls;
    [ObservableProperty] private bool isLoading;

    public void LoadFromVenue(VenueResponseDto venue)
    {
        VenueId = venue.Id;
        Name = venue.Name;
        Street = venue.Street;
        Number = venue.Number;
        Complement = venue.Complement;
        Neighborhood = venue.Neighborhood;
        City = venue.City;
        State = venue.State;
        PostalCode = venue.PostalCode;
        Latitude = venue.Latitude;
        Longitude = venue.Longitude;
        Description = venue.Description;
        Capacity = venue.Capacity;
        Rules = venue.Rules;
        VenueTypeId = venue.VenueTypeId;
        ImageUrls = new ObservableCollection<string>(venue.ImageUrls);
    }

    [RelayCommand]
    public async Task SelecionarImagensAsync()
    {
        try
        {
            var result = await FilePicker.PickMultipleAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images
            });

            if (result is null) return;

            IsLoading = true;
            var files = result.ToList();
            var upload = await _imageService.UploadVenuesImageAsync(files);
            ImageUrls.Clear();

            foreach (var image in upload.Select(i => i.Image))
                ImageUrls.Add(image);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", $"Falha ao fazer upload: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task UpdateVenueAsync()
    {
        try
        {
            IsLoading = true;

            var dto = new UpdateVenueRequestDto
            {
                Name = Name,
                Street = Street,
                Number = Number,
                Complement = Complement,
                Neighborhood = Neighborhood,
                City = City,
                State = State,
                PostalCode = PostalCode,
                Latitude = Latitude,
                Longitude = Longitude,
                Description = Description,
                Capacity = Capacity,
                Rules = Rules,
                VenueTypeId = VenueTypeId,
                ImageUrls = ImageUrls.ToList()
            };

            await _venueService.UpdateVenueAsync(VenueId, dto);
            await Shell.Current.DisplayAlert("Sucesso", "Local atualizado com sucesso!", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", $"Falha ao atualizar local: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }
}
