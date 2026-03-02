pipeline {
    agent any

    tools {
        dotnetsdk 'dotnet8'
    }

    stages {

        stage('Clean') {
            steps {
                sh 'dotnet clean UrlHealthMonitor.sln'
                sh 'rm -rf **/bin **/obj || true'
            }
        }

        stage('Restore') {
            steps {
                sh 'dotnet restore UrlHealthMonitor.sln'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build UrlHealthMonitor.sln --no-restore'
            }
        }

        stage('Test') {
            steps {
                sh 'dotnet test UrlHealthMonitor.sln --no-build --logger "junit;LogFilePath=test-results.xml"'
            }
            post {
                always {
                    junit '**/test-results.xml'
                }
            }
        }
    }

    post {
        success {
            echo 'Build and Tests Successful ✅'
        }
        failure {
            echo 'Build Failed ❌'
        }
    }
}
