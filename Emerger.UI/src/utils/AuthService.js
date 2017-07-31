import axios from 'axios';

const BASE_URL = 'http://localhost/Emerger.WebAPI';
const TOKEN_KEY = 'emerger_token';
const PROFILE_KEY = 'profile';

export default class AuthService {

  login(username, password) {
      return this.fetch(`${BASE_URL}/api/authentication/login/${username}/${password}`, {
        method: 'POST'
      })
      .then(res => {
          this.setJwtToken(res.token);
          this.setProfile(res.profile);
          axios.defaults.headers.common['Authorization'] = 'Bearer ' + this.getJwtToken();
          axios.defaults.headers.common['User'] = this.getProfile().id;
      });
  }

  loggedIn() {
    const idToken = this.getJwtToken();
    return !!idToken;
  }

  logout() {
      localStorage.removeItem(TOKEN_KEY);
      localStorage.removeItem(PROFILE_KEY);
  }

  getJwtToken() {
    return localStorage.getItem(TOKEN_KEY);
  }

  setJwtToken(token) {
    localStorage.setItem(TOKEN_KEY, token);
  }

  setProfile(profile) {
    const jsonProfile = JSON.stringify(profile);
    localStorage.setItem(PROFILE_KEY, jsonProfile);
  }

  getProfile(){
    // Retrieves the profile data from localStorage
    const profile = localStorage.getItem(PROFILE_KEY);
    return profile ? JSON.parse(localStorage.profile) : {};
  }

  _checkStatus(response) {
    // raises an error in case response status is not a success
    if (response.status >= 200 && response.status < 300) {
      return response;
    } else {
      var error = new Error(response.statusText);
      error.response = response;
      throw error;
    }
  }

  fetch(url, options){
      // performs api calls sending the required authentication headers
      const headers = {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      };

      if (this.loggedIn()){
        headers['Authorization'] = 'Bearer ' + this.getToken();
      }

      return fetch(url, {
        headers,
        ...options
      })
      .then(this._checkStatus)
      .then(response => response.json())
  }

}