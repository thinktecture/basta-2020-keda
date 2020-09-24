#!/bin/bash

pcs=$(az storage account show-connection-string -n basta2020keda -g rg-basta-2020-keda -o tsv)

kubectl create secret generic az-storage-auth --namespace message-transformer --from-literal=BastaStorageAccount="$pcs"
kubectl create secret generic az-storage-auth --namespace message-dispatcher --from-literal=BastaStorageAccount="$pcs" 