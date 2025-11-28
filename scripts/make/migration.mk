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
