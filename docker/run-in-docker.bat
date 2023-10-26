@echo off
docker-compose -f .\docker-compose.yml up --build --detach
echo "Local environment has started."
pause
docker-compose -f .\docker-compose.yml down