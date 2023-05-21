#!/usr/bin/env bash

cd deployment/rabbit-mq/
docker-compose --env-file .env.dev down

cd ../postgresql/
docker-compose --env-file .env.dev down

cd ../seq/
docker-compose --env-file .env.dev down

