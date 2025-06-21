namespace acheesporte_locator_app.Messages;

public record LocationSelectedMessage((double Latitude, double Longitude) Location);
