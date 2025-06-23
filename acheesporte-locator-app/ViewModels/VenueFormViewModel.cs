using acheesporte_locator_app.Dtos.VenueDtos;
using acheesporte_locator_app.Dtos.VenueTypeDtos;
using acheesporte_locator_app.Helpers;
using acheesporte_locator_app.Interfaces;
using acheesporte_locator_app.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace acheesporte_locator_app.ViewModels;

public partial class VenueFormViewModel : ObservableObject
{
    private readonly IVenueService _venueService;
    private readonly IImageInterface _imageService;
    private readonly ICepService _viaCepService;
    private readonly IVenueTypeService _venueTypeService;

    public VenueFormViewModel(
        IVenueService venueService,
        IImageInterface imageService,
        ICepService viaCepService,
        IVenueTypeService venueTypeService)
    {
        _venueService = venueService;
        _imageService = imageService;
        _viaCepService = viaCepService;
        _venueTypeService = venueTypeService;

        RegisterMessages(); 


        ImageUrls = new ObservableCollection<string>();
        VenueTypes = new ObservableCollection<VenueTypeResponseDto>();
    }

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
    [ObservableProperty] private ObservableCollection<VenueTypeResponseDto> venueTypes;
    [ObservableProperty] private VenueTypeResponseDto selectedVenueType;

    partial void OnSelectedVenueTypeChanged(VenueTypeResponseDto value)
    {
        if (value != null)
            VenueTypeId = value.Id;
    }

    [RelayCommand]
    public async Task LoadVenueTypesAsync()
    {
        try
        {
            IsLoading = true;

            var result = await _venueTypeService.GetVenueTypesAsync();
            VenueTypes.Clear();

            foreach (var type in result)
                VenueTypes.Add(type);

            if (VenueTypes.Count == 0)
            {
                await Shell.Current.DisplayAlert("Aviso", "Nenhum tipo de local encontrado.", "OK");

            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar tipos de local: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
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
    public async Task FindAddressByPostalCode()
    {
        if (string.IsNullOrWhiteSpace(PostalCode)) return;

        try
        {
            var address = await _viaCepService.GetAddressByCepAsync(PostalCode);
            if (address != null)
            {
                Street = address.Street;
                Neighborhood = address.Neighborhood;
                City = address.City;
                State = address.State;
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

            if (UserSession.CurrentUser is null)
            {
                await Shell.Current.DisplayAlert("Erro", "Usuário não autenticado. Faça login novamente.", "OK");
                return;
            }

            OwnerId = UserSession.CurrentUser.Id;

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
    private void RegisterMessages()
    {
        WeakReferenceMessenger.Default.Register<LocationSelectedMessage>(this, (r, m) =>
        {
            Latitude = m.Location.Latitude;
            Longitude = m.Location.Longitude;
        });
    }
}
