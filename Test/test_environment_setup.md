# Setting Up PostgreSQL Database and Connecting with JetBrains Rider

## Step 1: Pull the PostgreSQL Docker Image

Open your terminal and pull the PostgreSQL Docker image:

```sh
docker pull postgres
```

## Step 2: Run the PostgreSQL Container

Run a new PostgreSQL container with a different port and the desired database name:

```sh
docker run --name test -e POSTGRES_PASSWORD=test -d -p 5433:5432 postgres
```

- This command sets the PostgreSQL superuser password to `test` and maps container port `5432` to host port `5433`.
- The database named `HerbertTestDatabase` will be created automatically when you first connect.

## Step 3: Open JetBrains Rider

- Launch JetBrains Rider and open your project or workspace.

## Step 4: Open the Database Tool Window

- Go to **View > Tool Windows > Database** (or use the shortcut `Alt + 1`).

## Step 5: Add a New Data Source

- In the **Database** tool window, click the **+** icon (Add).
- Select **Data Source > PostgreSQL** from the dropdown menu.

## Step 6: Configure Database Connection

In the **Data Sources and Drivers** dialog, fill in the connection details:

- **Host**: `localhost`
- **Port**: `5433`
- **Database**: `HerbertTestDatabase`
- **User**: `postgres`
- **Password**: `test`

## Step 7: Test the Connection

- Click the **Test Connection** button.
- If successful, you will see a success message. If there are errors, ensure your Docker container is running and the settings are correct.

## Step 8: Save and Connect

- Once the connection test is successful, click **OK** to save the connection.
- Your `HerbertTestDatabase` should now appear in the **Database** tool window.
