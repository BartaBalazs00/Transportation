# 🚗 Transportation API

Egy ASP.NET Core Web API projekt járművek kezelésére és utazás-ajánlások generálására.

---

## 🔧 Használt technológiák

- **ASP.NET Core Web API**
- **MSSQL Local DB**
- **C# programozási nyelv**

---

## 🏗️ Felépítés

A projekt rétegzett architektúrára épül, az alábbi rétegekkel:

- `Data`
- `Entities`
- `Logic`
- `Endpoint`

---

## 📁 Rétegek részletesen

### 📦 Data

- Csak az `Entities` réteget ismeri.
- **TransportationContext.cs** – az in-memory adatbázis kezelésért felel.
- **Repository.cs** – általános CRUD műveletek bármely entitásra.

---

### 🚘 Endpoint

- Minden réteget ismer.
- **VehicleController.cs** – kezeli a HTTP kéréseket, és továbbítja azokat a `Logic` rétegnek.
  
#### API végpontok:

| Módszer | Útvonal | Leírás |
|--------|---------|--------|
| `GET` | `/vehicle` | Összes jármű lekérdezése |
| `GET` | `/vehicle/{id}` | Egy jármű lekérdezése ID alapján |
| `POST` | `/vehicle` | Új jármű létrehozása DTO alapján |
| `PUT` | `/vehicle/{id}` | Létező jármű frissítése |
| `DELETE` | `/vehicle/{id}` | Jármű törlése ID alapján |
| `POST` | `/vehicle/trip-suggestions` | Javasolt járművek listázása utas- és távolságigény alapján |

- **Program.cs** – rendszer konfigurációja, szolgáltatások regisztrálása, alkalmazás indítása.

---

### 📄 Entities

- DTO-k és a `Vehicle` entitás definíciója.

---

### 🧠 Logic

- Ismeri a `Data` és `Entities` rétegeket.
  
#### Fájlok:
- **DtoProvider.cs**
  - Mapper használata: `Vehicle` ↔ `VehicleViewDto` / `VehicleCreateUpdateDto`
  
- **VehicleLogic.cs**
  - Üzleti logika megvalósítása
  - CRUD műveletek DTO-kon keresztül
  - **GetTripSuggestion(int passengers, int distance):**
    - Szűri a járműveket hatótáv alapján
    - Generál járműkombinációkat az utasok számához
    - Kiszámolja minden kombináció profitját
    - Visszaadja a legjobb 1000 javaslatot
    - Használt metódusok:
      - GenerateCombinationsBasedOnK(List<Vehicle> vehicles, int k)
        - Kombinációk generálása adott méret alapján
      - CalculateProfit(List<Vehicle> vehicles, int distance, int passengers)
        - Profit számítása az adott kombinációkra

---

## ✅ Példák és Tesztelés

Swagger UI-val tesztelhető: `https://localhost:{port}/swagger`

---

## 📌 Megjegyzés

A rendszer in-memory adatkezelést használ, így minden újraindítás után törlődnek az adatok.

---

