import axios from 'axios'
import { useAuth } from '../hooks/useAuth';

console.log(process.env)
const AUTH_SERVICE_BASE_URL = process.env.REACT_APP_BUDGET_BUDDY_BASE_URL;
const USER_SERVICE_BASE_URL = process.env.REACT_APP_BUDGET_BUDDY_BASE_URL;


console.log(`Base URL: ${USER_SERVICE_BASE_URL}`);

const authService = axios.create({
    baseURL: AUTH_SERVICE_BASE_URL,
    headers: {
        "Content-Type": "application/json",
    },
    timeout: 30000,
});
const userService = axios.create({
    baseURL: AUTH_SERVICE_BASE_URL,
    headers: {
        "Content-Type": "application/json",
    },
    timeout: 30000,
});

userService.interceptors.request.use()


const apiRequest = {
    userService,
    authService,
}

export default apiRequest;