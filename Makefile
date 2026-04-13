PERSISTANCE_PROJECT = src/Infrastructure/BimManagerPortal.Persistance
WEBAPI_PROJECT      = src/Presentation/BimManagerPortal.WebApi
WASM_PROJECT        = src/Presentation/BimManagerPortal.WebAssembly

# ============================================================
#  DOCKER — локальный запуск
# ============================================================

## Собрать и запустить все контейнеры (пересборка образов)
up:
	docker compose -f compose.local.yaml up --build

## Запустить контейнеры без пересборки (быстро, если код не менялся)
start:
	docker compose -f compose.local.yaml up

## Остановить и удалить контейнеры
down:
	docker compose -f compose.local.yaml down

## Остановить, удалить контейнеры и тома с данными БД
down-volumes:
	docker compose -f compose.local.yaml down -v

## Пересобрать только образ api
rebuild-api:
	docker compose -f compose.local.yaml build api

## Пересобрать только образ web
rebuild-web:
	docker compose -f compose.local.yaml build web

## Логи всех контейнеров (следить в реальном времени)
logs:
	docker compose -f compose.local.yaml logs -f

## Логи только api
logs-api:
	docker compose -f compose.local.yaml logs -f api

## Логи только web
logs-web:
	docker compose -f compose.local.yaml logs -f web

## Статус контейнеров
ps:
	docker compose -f compose.local.yaml ps

# ============================================================
#  EF CORE — миграции
# ============================================================

## Создать новую миграцию: make migration NAME=НазваниеМиграции
migration:
	dotnet ef migrations add $(NAME) \
		--project $(PERSISTANCE_PROJECT) \
		--startup-project $(WEBAPI_PROJECT)

## Применить все миграции к базе данных
db-update:
	dotnet ef database update \
		--project $(PERSISTANCE_PROJECT) \
		--startup-project $(WEBAPI_PROJECT)

## Откатить последнюю миграцию (удалить файл миграции)
migration-remove:
	dotnet ef migrations remove \
		--project $(PERSISTANCE_PROJECT) \
		--startup-project $(WEBAPI_PROJECT)

## Показать список всех миграций и их статус
migrations-list:
	dotnet ef migrations list \
		--project $(PERSISTANCE_PROJECT) \
		--startup-project $(WEBAPI_PROJECT)

# ============================================================
#  СБОРКА
# ============================================================

## Собрать весь solution
build:
	dotnet build

## Очистить артефакты сборки
clean:
	dotnet clean

## Собрать только WebApi
build-api:
	dotnet build $(WEBAPI_PROJECT)

## Собрать только WebAssembly
build-web:
	dotnet build $(WASM_PROJECT)

## Восстановить NuGet-пакеты
restore:
	dotnet restore

.PHONY: up start down down-volumes rebuild-api rebuild-web logs logs-api logs-web ps \
        migration db-update migration-remove migrations-list \
        build clean build-api build-web restore
