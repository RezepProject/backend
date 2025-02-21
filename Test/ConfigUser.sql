CREATE TABLE role (
                      id SERIAL PRIMARY KEY,                -- Auto-incrementing primary key
                      name TEXT NOT NULL UNIQUE              -- Name property (assuming unique for role names)
);

CREATE TABLE configuser (
                            id SERIAL PRIMARY KEY,                 -- Auto-incrementing primary key
                            first_name TEXT NOT NULL,              -- FirstName property
                            last_name TEXT NOT NULL,               -- LastName property
                            email TEXT NOT NULL UNIQUE,            -- Email property (assuming unique)
                            password TEXT NOT NULL,                -- Password property
                            role_id INTEGER NOT NULL,              -- RoleId property
                            refresh_token TEXT NOT NULL,           -- RefreshToken property
                            token_created TIMESTAMPTZ NOT NULL,   -- TokenCreated property (timestamp with time zone)
                            token_expires TIMESTAMPTZ NOT NULL,   -- TokenExpires property (timestamp with time zone)
                            CONSTRAINT fk_configuser_roles_role_id FOREIGN KEY (role_id) REFERENCES role(id) ON DELETE CASCADE -- Foreign key constraint referencing Role table
);
