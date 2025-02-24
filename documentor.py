import os
import openai

# OpenAI-Client erstellen
client = openai.OpenAI(api_key="")

if not client.api_key:
    raise ValueError("Bitte füge deinen OpenAI-API-Schlüssel hinzu.")

# Funktion zum Einlesen von C#-Dateien im Projekt
def read_csharp_files(directory):
    csharp_code = []
    for root, _, files in os.walk(directory):
        for file in files:
            if file.endswith(".cs"):
                file_path = os.path.join(root, file)
                with open(file_path, "r", encoding="utf-8") as f:
                    csharp_code.append(f"\n// Datei: {file_path}\n" + f.read() + "\n\n")
    return csharp_code

# C#-Code in kleinere Teile unterteilen
def split_code_into_parts(csharp_code, max_tokens=8000):
    parts = []
    current_part = ""
    for code in csharp_code:
        # Füge den Code hinzu, bis die Tokenanzahl überschritten wird
        if len(current_part) + len(code) > max_tokens:
            parts.append(current_part)
            current_part = code
        else:
            current_part += code
    if current_part:
        parts.append(current_part)  # Der letzte Teil
    return parts

# 📌 Hier dein Projektverzeichnis anpassen
project_dir = "Backend"

# C#-Code aus den Dateien einlesen
csharp_code = read_csharp_files(project_dir)

# Den Code in kleinere Teile unterteilen
code_parts = split_code_into_parts(csharp_code)

# Speicherort für die generierte Dokumentation
output_file = "documentation.md"

# Datei zur Speicherung der gesamten Dokumentation öffnen
with open(output_file, "w", encoding="utf-8") as file:
    # Jede Teilanfrage an OpenAI
    for part in code_parts:
        response = client.chat.completions.create(
            model="gpt-3.5-turbo",
            messages=[
                {"role": "system", "content": "Generate professional documentation in Markdown for this C# project. The documentation should include a description of the project, its purpose, and the classes and methods used. It should also explain the endpoints and data structures used. The reader knows about programming and C# but is not familiar with the project."},
                {"role": "user", "content": part}
            ]
        )
        # Generierte Dokumentation an die Datei anhängen
        documentation = response.choices[0].message.content
        file.write(documentation)

print(f"Dokumentation erfolgreich in {output_file} gespeichert.")
