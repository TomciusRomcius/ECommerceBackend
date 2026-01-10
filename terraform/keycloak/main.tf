variable "keycloak_admin_username" {
    type = string
    description = "Keycloak admin username"
}

variable "keycloak_admin_password" {
    type = string
    description = "Keycloak admin password"
}

variable "keycloak_url" {
    type = string
    description = "Keycloak URL"
}

variable "ecommerce_api_client_secret" {
    type = string
    description = "Client secret of the ecommerce-api secret"
}

terraform {
  required_providers {
    keycloak = {
      source  = "mrparkers/keycloak"
      version = ">= 4.4.0"
    }
  }
}

provider "keycloak" {
    client_id     = "admin-cli"
    username = var.keycloak_admin_username
    password = var.keycloak_admin_password
    url           = var.keycloak_url
}

resource "keycloak_realm" "ecommerce_api" {
  realm             = "ecommerce-api"
  enabled           = true
  display_name      = "Ecommerce API"
}

resource "keycloak_openid_client" "openid_client" {
  realm_id            = keycloak_realm.ecommerce_api.id
  client_id           = "ecommerce-api"
  client_secret = var.ecommerce_api_client_secret

  name                = "Ecommerce API"
  enabled             = true

  access_type         = "CONFIDENTIAL"
  service_accounts_enabled = true
}

resource "keycloak_role" "ecommerce_admin_role" {
  realm_id    = keycloak_realm.ecommerce_api.id
  name        = "ecommerce-admin"
}

resource "keycloak_openid_hardcoded_role_protocol_mapper" "hardcoded_admin_role" {
  realm_id  = keycloak_realm.ecommerce_api.id
  client_id = keycloak_openid_client.openid_client.id
  name      = "hardcoded-admin-role"
  
  role_id = keycloak_role.ecommerce_admin_role.id
}

# Protocol mapper to include realm roles in the token
resource "keycloak_openid_user_realm_role_protocol_mapper" "realm_roles_mapper" {
  realm_id  = keycloak_realm.ecommerce_api.id
  client_id = keycloak_openid_client.openid_client.id
  name      = "realm-roles-mapper"
  
  claim_name       = "roles"
  multivalued      = true
  claim_value_type = "String"
}

resource "keycloak_openid_client_scope" "audience_client_scope" {
  realm_id = keycloak_realm.ecommerce_api.id
  name     = "audience-client-scope"
}

resource "keycloak_openid_audience_protocol_mapper" "audience_mapper" {
  realm_id        = keycloak_realm.ecommerce_api.id
  client_scope_id = keycloak_openid_client_scope.audience_client_scope.id
  name            = "audience-mapper"
  included_client_audience = keycloak_openid_client.openid_client.client_id
}

resource "keycloak_openid_client_default_scopes" "ecommerce_api_client_default_scopes" {
  realm_id  = "ecommerce-api"
  client_id = keycloak_openid_client.openid_client.id

  default_scopes = [
    "profile",
    "email",
    keycloak_openid_client_scope.audience_client_scope.name,
  ]
}