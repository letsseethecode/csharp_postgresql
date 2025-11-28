.PHONY: up down pgadmin build grate grate--interactive
GOAL: help

ifndef VERBOSE
.SILENT:
endif

SHELL := /bin/bash
VERSION ?= latest

include scripts/make/*.mk

help: 																			## Show this help message
	@echo -e "Usage: make [target]"
	@echo
	@echo -e "where [target] can be:"
	@grep -hE '^[a-zA-Z_-]+:.*## .*$$' ${MAKEFILE_LIST} \
		| sort \
		| awk 'BEGIN {FS = ":.*?## "}; {printf "${CYAN}%s${RESET}~%s\n", $$1, $$2}' \
		| column -t -s~ \
		| sed 's/^/  /'
	@echo

up: 																			## Start the postgres and pgadmin containers
	$(call highlight,"services","starting...")
	docker compose up pgadmin postgres -d

down: 																			## Stop all the containers
	$(call highlight,"services","stopping...")
	docker compose down -v

clean: 																			## Remove the temporary build files
	$(call highlight,"project","cleaning...")
	rm -rf dist/

build: build--api build--migration ## Build the grate container
	$(call highlight,"build","finished")

# Tests must be marked with [Trait("Category", "Unit")] in xUnit or @Unit in Specflow
test--unit:																		## Run the unit tests
	$(call highlight,"test","unit tests complete")
	dotnet test \
		--logger "console;verbosity=detailed"
		--filter "Category=Unit"

# Tests that are not marked as Unit tests (above) will be treated as integration tests.
test--integration:																## Run the integration tests
	$(call highlight,"test","integration tests complete")
	dotnet test \
		--logger "console;verbosity=detailed" \
		--filter "Category!=Unit"

test: test--unit test--integration									## Run all the tests
	$(call highlight,"test","complete")

wipe:																			## Remove the build and docker files (DANGER!)
	$(call confirm,"wipe","This will remove all local content and all docker images")
	read answer; \
	if [[ "$${answer}" != "y" ]]; then \
		$(call err,"User aborted."); \
		exit 1; \
	fi
	$(call highlight,"wipe","remove all docker content...")
	docker kill $$(docker ps -q) 			2> /dev/null || true
	docker rm $$(docker container ls -aq)	2> /dev/null || true
	docker rmi $$(docker images -q) --force	2> /dev/null || true
	$(call highlight,"wipe","removing local directories...")
	rm -rf dist/
	rm -rf volumes/  							2> /dev/null || true
	$(call highlight,"wipe","complete")
