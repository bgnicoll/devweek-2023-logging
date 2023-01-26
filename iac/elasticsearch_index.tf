terraform {
  required_providers {
    elasticsearch = {
      source = "phillbaker/elasticsearch"
      version = "2.0.7"
    }
  }
}
provider "elasticsearch" {
  url = "https://127.0.0.1:9200"
  username = "elastic"
  password = "elastic"
  healthcheck = false
  insecure = true
}

resource "elasticsearch_index" "log-generator-5000" {
  name               = "log-generator-5000"
  number_of_shards   = 1
  number_of_replicas = 1
}
