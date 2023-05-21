#!/usr/bin/env bash

cd deployment/rabbit-mq/
ls -a
docker-compose --env-file .env.dev up -d

cd ../postgresql/
docker-compose --env-file .env.dev up -d

cd ../seq/
docker-compose --env-file .env.dev up -d


