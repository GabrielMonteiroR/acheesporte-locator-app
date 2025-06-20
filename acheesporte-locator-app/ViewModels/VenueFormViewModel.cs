using acheesporte_locator_app.Dtos.VenueDtos;
using acheesporte_locator_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace acheesporte_locator_app.ViewModels;

public partial class VenueRegisterViewModel : ObservableObject
{
    private readonly IVenueService _venueService;
    private readonly IImageInterface _imageService;
    private readonly ICepService _viaCepService;

    public VenueRegisterViewModel(IVenueService venueService, IImageInterface imageService, ICepService viaCepService)
    {
        _venueService = venueService;
        _imageService = imageService;
        _viaCepService = viaCepService;

        ImageUrls = new ObservableCollection<string>();
    }

    // Campos do formulário
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
    [ObservableProperty] private int ownerId;

    [ObservableProperty] private ObservableCollection<string> imageUrls;

    [ObservableProperty] private bool isLoading;

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
            {
                ImageUrls.Add(image);
            }
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
    public async Task FindAddressByPostalCode()
    {
        if (string.IsNullOrWhiteSpace(PostalCode)) return;

        try
        {
            var Adress = await _viaCepService.GetAddressByCepAsync(PostalCode);
            if (Adress != null)
            {
                Street = Adress.Street;
                Neighborhood = Adress.Neighborhood;
                City = Adress.City;
                State = Adress.State;
            }
        }
        catch
        {
            await Shell.Current.DisplayAlert("Erro", "Falha ao buscar endereço pelo CEP.", "OK");
        }
    }

    [RelayCommand]
    public async Task SelectLocationOnMap()
    {
       await Shell.Current.GoToAsync("MapSelectPage", true);
    }

    [RelayCommand]
    public async Task RegisterVenueAsync()
    {
        try
        {
            IsLoading = true;

            var dto = new CreateVenueRequestDto
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
                OwnerId = OwnerId,
                ImageUrls = ImageUrls.ToList()
            };

            await _venueService.CreateVenueAsync(dto);
            await Shell.Current.DisplayAlert("Sucesso", "Local cadastrado com sucesso!", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", $"Falha ao cadastrar local: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }
}
