.PHONY: up down pgadmin build grate grate--interactive

ifndef VERBOSE
.SILENT:
endif

SHELL := /bin/bash
VERSION ?= latest

help: ## Show this help message
	@echo -e "Usage: make [target]"
	@echo
	@echo -e "where [target] can be:"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "  \033[36m%-30s\033[0m %s\n", $$1, $$2}'

up: ## Start the postgres and pgadmin containers
	docker compose up pgadmin postgres -d

down: ## Stop all the containers
	docker compose down -v

clean: # Remove the temporary files
	rm -rf dist/

dist/grate.zip:
	echo -e "\033[33m>>> Downloading Grate... <<<\033[0m"
	mkdir -p $(dir $@)
	curl --output dist/grate.zip \
		--location https://github.com/erikbra/grate/releases/download/1.8.0/grate-linux-x64-self-contained-1.8.0.zip

dist/grate: dist/grate.zip
	echo -e "\033[33m>>> Unzipping Grate... <<<\033[0m"
	unzip -o $< -d $(dir $@)
	touch $@

GRATE_SQL_FILES := $(find ./grate-sql -name "*.sql" -type f)

dist/docker-grate: Dockerfile ./entrypoint.sh dist/grate ${GRATE_SQL_FILES}
	echo -e "\033[33m>>> Building Docker Image... <<<\033[0m"
	mkdir -p $(dir $@)
	cd migration; 					\
	docker build . 					\
		--platform "linux/amd64" 	\
		-t "migration:${VERSION}"; 	\
	touch $@

build: dist/docker-grate ## Build the grate container

migration: dist/docker-grate ## Run the grate container
	echo -e "\033[33m>>> Running Docker Migration... <<<\033[0m"
	docker compose run --remove-orphans grate

migration--interactive: dist/docker-grate ## Run the grate container in a terminal
	echo -e "\033[33m>>> Running Docker Container Interactively\033[0m"
	docker compose run -it --remove-orphans grate /bin/bash
