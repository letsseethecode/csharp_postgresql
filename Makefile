.PHONY: up down pgadmin build grate grate--interactive
GOAL: help

ifndef VERBOSE
.SILENT:
endif

SHELL := /bin/bash
VERSION ?= latest

include scripts/make/*.mk

help: ## Show this help message
	@echo -e "Usage: make [target]"
	@echo
	@echo -e "where [target] can be:"
	@grep -hE '^[a-zA-Z_-]+:.*## .*$$' ${MAKEFILE_LIST} \
		| sort \
		| awk 'BEGIN {FS = ":.*?## "}; {printf "${CYAN}%s${RESET}~%s\n", $$1, $$2}' \
		| column -t -s~ \
		| sed 's/^/  /'
	@echo

up: ## Start the postgres and pgadmin containers
	$(call highlight,"services","starting...")
	docker compose up pgadmin postgres -d

down: ## Stop all the containers
	$(call highlight,"services","stopping...")
	docker compose down -v

clean: # Remove the temporary files
	$(call highlight,"project","cleaning...")
	rm -rf dist/

build: build--api build--migration ## Build the grate container
	$(call highlight,"build","finished")

test--unit:																		## Run the unit tests
	$(call highlight,"test","unit tests complete")

test--integration:																## Run the integration tests
	$(call highlight,"test","integration tests complete")

test--bdd:																		## Run the BDD tests
	$(call highlight,"test","bdd tests complete")

test: test--unit test--bdd test--integration									## Run all the tests
	$(call highlight,"test","complete")
