
# 🛡️ SafeWalk SA  
### *A community-driven personal safety companion for South Africa.*

SafeWalk SA is an open-source safety application designed to help South Africans travel with confidence by providing **journey check-ins**, **live location sharing**, and **emergency alerts** to trusted contacts.  
It’s built with a **privacy-first mindset**, a **clean architecture**, and real-world relevance for communities facing daily safety risks.

---

## 🌍 Why SafeWalk SA Exists
South Africa faces unique and serious personal safety challenges.  
Walking home from work, attending night classes, or commuting early in the morning can create anxiety and real danger.

SafeWalk SA aims to provide:
- **Prevention**, not just reaction  
- **Connection**, not surveillance  
- **Simple, reliable tools** anyone can use  
- **Data-light support** suitable for all communities  

This project is created to make safety accessible, empowering, and community-driven.

---

## 🔑 Core Features (MVP)

### ✔️ Journey Check-In
Users set:
- Destination  
- Estimated arrival time  
- Trusted contacts  
If the user doesn’t arrive in time and doesn’t check in, the app **automatically notifies** their trusted circle.

---

### ✔️ Live Location Sharing
Share your real-time location:
- Temporarily  
- With selected contacts  
- Even via SMS link (low-data mode)

---

### ✔️ Panic Button
One tap sends:
- Live location  
- Emergency message  
- Time of alert  
Directly to the user’s trusted circle.

---

### ✔️ Trusted Circle
Each user selects 3–5 trusted people who:
- Receive alerts  
- Can monitor active journeys  
- Are notified only with consent  

---

### ✔️ SA Emergency Resource Hub
Quick access to:
- SAPS emergency numbers  
- Campus security  
- GBV helplines  
- Community groups  

---

Key benefits:
- Separation of concerns  
- Testability  
- Scalability  
- Maintainability  

---

## 🧩 Tech Stack

### Backend
- .NET 9 Web API  
- Clean Architecture  
- EF Core for data access  
- Azure SQL Database  
- Azure Maps or Google Maps API  
- Twilio or Clickatell for SMS alerts  
- Firebase for push notifications (optional)

### Security
- JWT Authentication  
- Role-based access  
- Privacy-first location handling (no long-term tracking)

### Deployment
- Azure App Service  
- CI/CD via GitHub Actions (planned)

---

## 🚀 Planned Roadmap

### Phase 1 – MVP
- User registration + login  
- Trusted circle management  
- Start/stop journey  
- Auto-expiring check-ins  
- Panic alert system  
- Basic UI (web/mobile-ready)

### Phase 2 – Mobile-first
- Convert to Progressive Web App (PWA)  
- Offline mode  
- Push notifications  

### Phase 3 – Community Integration
- Partnerships with campuses  
- Community safety patrols  
- Anonymous incident reporting  

### Phase 4 – Advanced Features
- Predictive route risk scoring (AI)  
- Heatmaps of unsafe routes  
- Bluetooth beacon integration  
- Safe taxi verification system  

---

## 💙 Ethical Foundations & Privacy

SafeWalk SA follows strict principles:
- **No constant tracking**
- **Location data is temporary**
- **Users own their data**
- **Only trusted contacts receive alerts**
- **No data is sold, ever**

This app exists to *protect people, not monitor them*.




