.PHONY: up down pgadmin build grate grate--interactive

ifndef VERBOSE
.SILENT:
endif

ARR 		= 
WHITE_BG	= \033[7;37m
RED_BG		= \033[7;31m
GREEN_BG	= \033[7;32m
YELLOW_BG	= \033[7;33m
BLUE_BG		= \033[7;34m
PURPLE_BG	= \033[7;35m
CYAN_BG		= \033[7;36m

RED		= \033[0;31m
GREEN	= \033[0;32m
YELLOW	= \033[0;33m
BLUE	= \033[0;34m
PURPLE	= \033[0;35m
CYAN	= \033[0;36m

RESET	= \033[0m

SHELL := /bin/bash
VERSION ?= latest

highlight = echo -e "${CYAN_BG} $(1) ${CYAN}${RESET} $(2)"

help: ## Show this help message
	@echo -e "Usage: make [target]"
	@echo
	@echo -e "where [target] can be:"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "  ${CYAN}%-30s${RESET} %s\n", $$1, $$2}'

up: ## Start the postgres and pgadmin containers
	$(call highlight,"services","starting...")
	docker compose up pgadmin postgres -d

down: ## Stop all the containers
	$(call highlight,"services","stopping...")
	docker compose down -v

clean: # Remove the temporary files
	$(call highlight,"project","cleaning...")
	rm -rf dist/

# ------------------------------------------------------------------------------
# Migration tool
# ------------------------------------------------------------------------------

dist/grate.zip:
	$(call highlight,"grate","downloading...")
	mkdir -p $(dir $@)
	curl --output dist/grate.zip \
		--location https://github.com/erikbra/grate/releases/download/1.8.0/grate-linux-x64-self-contained-1.8.0.zip

dist/grate: dist/grate.zip
	$(call highlight,"grate","unzipping...")
	unzip -o $< -d $(dir $@)
	touch $@

GRATE_SOURCE_FILES := $(find ./migration/sql -name Dockerfile -o -name "*.sh" -o -name "*.sql" -type f)
dist/docker/migration: dist/grate ${GRATE_SOURCE_FILES}
	$(call highlight,"migration","building...")
	docker build 					\
		--platform "linux/amd64" 	\
		-t migration:latest	 		\
		-f src/migration/Dockerfile	\
		.
	mkdir -p $(dir $@)
	touch $@

build--migration: dist/docker/migration											## Build the migration container

migration: build--migration 													## Run the grate container
	$(call highlight,"migration","running...")
	docker compose run --remove-orphans grate

migration--interactive: build--migration										## Run the grate container in a terminal
	$(call highlight,"migration","building...")
	docker compose run -it --remove-orphans grate /bin/bash

# ------------------------------------------------------------------------------
# API
# ------------------------------------------------------------------------------

API_SOURCE_FILES := $(find src/LSTC.CheeseShop.Api -name Dockerfile -o -name "*.cs" -o -name "*.csproj")
dist/docker/api: ${API_SOURCE_FILES}
	$(call highlight,"api","building...")
	docker build 								\
		--platform "linux/amd64"				\
		-t cheeseshop-api:latest 				\
		-f src/LSTC.CheeseShop.Api/Dockerfile	\
		.
	mkdir -p $(dir $@)
	touch $@

build--api: dist/docker/api														## Build the API

up--api:																		## Run the API
	$(call highlight,"api","running...")
	docker compose up api

# ------------------------------------------------------------------------------
# General
# ------------------------------------------------------------------------------

build: build--api build--migration ## Build the grate container
	$(call highlight,"build","finished")