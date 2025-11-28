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

up--api: build--api																## Run the API
	$(call highlight,"api","running...")
	docker compose up api
