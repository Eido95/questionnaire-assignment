# Questionnaire Assignment

Author: Eido Askayo

## Kubernetes Deployment

### Cluster Setup and Deployment

1. `minikube start -p minikube-01` (or use your favorite Kubernetes cluster)
2. `kubectl config current-context` - See `minikube-01`
3. Deploy database:
    1. `kubectl apply -f pv-mysql.yaml`
    2. `kubectl apply -f deployment-mysql.yaml`
    3. `kubectl get -f deployment-mysql.yaml` - Wait for it to be `READY 1/1` (takes ~30s)
4. Deploy service:
    1. `kubectl apply -f deployment-service.yaml`
    2. `kubectl get -f deployment-service.yaml` - Wait for it to be `READY 3/3` (takes ~20s)
5. Deploy UI:
    1. `kubectl apply -f deployment-ui.yaml`
    2. `kubectl get -f deployment-ui.yaml` - Wait for it to be `READY 1/1` (takes ~10s)
6. `minikube -p minikube-01 service --all` - Will open 2 browser tabs:
    1. `http://<service ip>:30000` - Questionnaire UI
    2. `http://<service ip>:30001/swagger` - Questionnaire Service API docs

### Cluster Teardown

1. `minikube -p minikube-01 stop`
2. `minikube -p minikube-01 delete`

## Contributing Guidelines

### Database

1. Database: MySQL Server

### Service

1. IDE: JetBrains Rider
2. Open project by selecting `Questionnaire.sln` file
3. Modify `Program.cs` according to your development environment:
    1. MySQL server connection details
    2. CORS settings
    3. Database initialization
4. Install NuGet packages
5. In Rider IDE click "Run"
6. Container build: `docker build -f QuestionnaireService/Dockerfile -t <tag> .`

### UI

1. IDE: Visual Studio Code
2. Open project by selecting `questionnaire-ui` directory
2. Modify `questionnaire.js` according to your development environment
    1. API host (Questionnaire Service) address
3. Run `npm install` in terminal
4. Run `npm run serve` in terminal
5. Container build: `docker build -t <tag> .`
