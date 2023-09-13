# Questionnaire Assignment

Author: Eido Askayo

## Kubernetes Deployment

### Cluster Setup and Deployment

1. `kubectl config current-context` - Make sure you use `minikube` Kubernetes cluster
2. `minikube start`
3. Deploy database
    1. `kubectl apply -f pv-mysql.yaml`
    2. `kubectl apply -f deployment-mysql.yaml`
    3. `kubectl get -f deployment-mysql.yaml` - Wait for it to be `READY 1/1`
4. Deploy service
    1. `kubectl apply -f deployment-service.yaml`
    2. `kubectl get -f deployment-service.yaml` - Wait for it to be `READY 1/1`
    3. The service will automatically populate initial database records
5. Deploy UI
    1. `kubectl apply -f deployment-ui.yaml`
    2. `kubectl get -f deployment-ui.yaml` - Wait for it to be `READY 1/1`
8. `minikube service --all` - Will open 2 browser tabs:
    1. `http://<service ip>:30000` - Questionnaire UI
    2. `http://<service ip>:30001/swagger` - Questionnaire Service API docs

### Cluster Teardown

1. `minikube stop`
2. `minikube delete`

## Contributing Guidelines

### Database

1. Database: MySQL

### Service

1. IDE: JetBrains Rider
2. Open project by selecting `Questionnaire.sln` file
3. Modify `Program.cs` according to your development environment:
    1. MySQL server connection details
    2. CORS settings
    3. Database initialization
4. Install NuGet packages
5. In Rider IDE click "Run"

### UI

1. IDE: Visual Studio Code
2. Open project by selecting `questionnaire-ui` directory
2. Modify `questionnaire.js` according to your development environment
    1. API host (Questionnaire Service) address
 3. Run `npm install` in terminal
 4. Run `npm run serve` in terminal
