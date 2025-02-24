# Tested Behaviour

# ✅ Tested Behaviors in `AnswerControllerTests`

- [x] `GET /answer`
- [x] `GET /answer/{id}` (existing)
- [x] `GET /answer/{id}` (non-existing)
- [x] `POST /answer`
- [x] `PUT /answer/{id}` (existing)
- [x] `PUT /answer/{id}` (non-existing)
- [x] `DELETE /answer/{id}` (existing)
- [x] `DELETE /answer/{id}` (non-existing)

# ✅ Tested Behaviors in `BackgroundImageControllerTests`

- [x] `GET /backgroundimage`
- [x] `GET /backgroundimage/{id}` (existing)
- [x] `GET /backgroundimage/{id}` (non-existing)
- [x] `POST /backgroundimage`
- [x] `DELETE /backgroundimage/{id}` (existing)
- [x] `DELETE /backgroundimage/{id}` (non-existing)

# ✅ Tested Behaviors in `ConfigControllerTests`

- [x] `GET /config`
- [x] `GET /config/{id}` (existing)
- [x] `GET /config/{id}` (non-existing)
- [x] `POST /config`
- [x] `PUT /config/{id}` (existing)
- [x] `PUT /config/{id}` (non-existing)
- [x] `DELETE /config/{id}` (existing)
- [x] `DELETE /config/{id}` (non-existing)

# ✅ Tested Behaviors in `ConfigUserControllerTests`

- [x] `GET /configuser`
- [x] `GET /configuser/{id}` (non-existing)
- [x] `DELETE /configuser/{id}` (existing)
- [x] `DELETE /configuser/{id}` (non-existing)
- [x] `PUT /configuser/{id}` (existing)
- [x] `PUT /configuser/{id}` (non-existing)
- [x] `POST /configuser/invitation/{id}/resend` (non-existing)
- [x] `DELETE /configuser/invitation/{id}` (non-existing)

# ✅ Tested Behaviors in `QuestionControllerTests`

- [x] `GET /question` (returns all questions)
- [x] `GET /question/{id}` (existing question)
- [x] `GET /question/{id}` (non-existing question)
- [x] `POST /question` (create new question)
- [x] `PUT /question/{id}` (update existing question)
- [x] `PUT /question/{id}` (non-existing question)
- [x] `DELETE /question/{id}` (existing question)
- [x] `DELETE /question/{id}` (non-existing question)
- [x] `GET /question/categories` (returns all categories)

# ✅ Tested Behaviors in `RoleControllerTests`

- [x] `GET /role` (returns all roles)
- [x] `GET /role/{id}` (existing role)
- [x] `GET /role/{id}` (non-existing role)
- [x] `POST /role` (create new role)
- [x] `PUT /role/{id}` (update existing role)
- [x] `PUT /role/{id}` (non-existing role)
- [x] `DELETE /role/{id}` (existing role)
- [x] `DELETE /role/{id}` (non-existing role)

# ✅ Tested Behaviors in `SettingsControllerTests`

- [x] `GET /settings` (returns all settings)
- [x] `GET /settings/{id}` (existing setting)
- [x] `GET /settings/{id}` (non-existing setting)
- [x] `POST /settings` (create new setting)
- [x] `PUT /settings/{id}` (update existing setting)
- [x] `PUT /settings/{id}` (non-existing setting)

# ✅ Tested Behaviors in `StreamingControllerTests`

- [x] `POST /streaming` (valid Base64 image → returns OK)
- [x] `POST /streaming` (invalid Base64 string → returns BadRequest)
- [x] `POST /streaming` (empty frame data → returns BadRequest)

# ✅ Tested Behaviors in `TaskControllerTests`

- [x] `GET /task` (returns all tasks)
- [x] `GET /task/{id}` (existing ID → returns correct task)
- [x] `GET /task/{id}` (non-existing ID → returns NotFound)
- [x] `POST /task` (creates a new task and returns it)
- [x] `PUT /task/{id}` (existing ID → updates task correctly)
- [x] `PUT /task/{id}` (non-existing ID → returns NotFound)
- [x] `DELETE /task/{id}` (existing ID → deletes task)
- [x] `DELETE /task/{id}` (non-existing ID → returns NotFound)



