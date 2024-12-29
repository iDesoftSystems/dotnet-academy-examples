# HelloWorldService

## Run your service

In your terminal, run the following command:

```bash
dotnet run
```

Wait for the app to display that it's listening on `http://localhost:<port number>`, and then open a browser and navigate to `http://localhost:<port number>/weatherforecast`.

Press `CTRL`+`C` on your terminal to end the `dotnet run` command that is running the service locally.

## Create the docker image

```bash
docker build -t helloworldservice .
```

The `docker build` command uses the `Dockerfile` to build a Docker image.

- The `-t helloworldservice` parameter tells it to tag (or name) the image as `helloworldservice`.
- The final parameter tells it which directory to use to find the `Dockerfile` (`.` specifies the current directory).
- This command will download and build all dependencies to create a Docker image and may take some time.

You can run the following command to see a list of all images available on your machine, including the one you just created.

## Run docker image

You can run your app in a container using the following command:

```bash
docker run -it --rm -p 3000:8080 --name helloworldservicecontainer helloworldservice
```

You can browse to the following URL to access your application running in a container: `http://localhost:3000/weatherforecast`.

Press `CTRL`+`C` on your terminal to end the docker run command that is running the service in a container.
