provider "azurerm" {
  features {
    resource_group {
      prevent_deletion_if_contains_resources = false
    }
  }
}

resource "random_pet" "random" {
  separator = ""
  length    = 2
}

resource "azurerm_resource_group" "rg" {
  name     = "rg-${random_pet.random.id}"
  location = var.location
}

resource "azurerm_signalr_service" "srs" {
  name                = "srs${random_pet.random.id}"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  sku {
    name     = "Free_F1"
    capacity = 1
  }

  cors {
    allowed_origins = ["*"]
  }

  live_trace {
    enabled = true
  }

  connectivity_logs_enabled = true
  messaging_logs_enabled    = true
  service_mode              = "Default"
}
