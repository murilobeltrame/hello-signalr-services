
resource "azurerm_resource_group" "example" {
  name     = "terraform-signalr"
  location = "West Europe"
}

resource "azurerm_signalr_service" "example" {
  name                = "tfex-signalr"
  location            = azurerm_resource_group.example.location
  resource_group_name = azurerm_resource_group.example.name

  sku {
    name     = "Free_F1"
    capacity = 1
  }

  cors {
    allowed_origins = ["http://www.example.com"]
  }

  connectivity_logs_enabled = true
  messaging_logs_enabled    = true
  service_mode              = "Default"

  upstream_endpoint {
    category_pattern = ["connections", "messages"]
    event_pattern    = ["*"]
    hub_pattern      = ["hub1"]
    url_template     = "http://foo.com"
  }
}
