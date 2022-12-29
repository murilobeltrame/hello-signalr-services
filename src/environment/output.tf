output "random_pet_name" {
  value = random_pet.random.id
}

output "SignalRServiceConnectionString" {
  sensitive = true
  value     = azurerm_signalr_service.srs.primary_connection_string
}
