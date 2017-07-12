import {login as apiLogin} from './Api'

export function login(username, password) {
    return apiLogin(username, password);
}

export function logout() {
  this.props.history.push('/');
}

export function requireAuth(nextState, replace) {
  if (!isLoggedIn()) {
    replace({pathname: '/'});
  }
}

// export function getIdToken() {
//   return localStorage.getItem(ID_TOKEN_KEY);
// }

// export function getAccessToken() {
//   return localStorage.getItem(ACCESS_TOKEN_KEY);
// }

// function clearIdToken() {
//   localStorage.removeItem(ID_TOKEN_KEY);
// }

// function clearAccessToken() {
//   localStorage.removeItem(ACCESS_TOKEN_KEY);
// }

// Helper function that will allow us to extract the access_token and id_token
// function getParameterByName(name) {
//   let match = RegExp('[#&]' + name + '=([^&]*)').exec(window.location.hash);
//   return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
// }

// // Get and store access_token in local storage
// export function setAccessToken() {
//   let accessToken = getParameterByName('access_token');
//   localStorage.setItem(ACCESS_TOKEN_KEY, accessToken);
// }

// // Get and store id_token in local storage
// export function setIdToken() {
//   let idToken = getParameterByName('id_token');
//   localStorage.setItem(ID_TOKEN_KEY, idToken);
// }

export function isLoggedIn() {
    return false;
//   const idToken = getIdToken();
//   return !!idToken && !isTokenExpired(idToken);
}

// function getTokenExpirationDate(encodedToken) {
//   const token = decode(encodedToken);
//   if (!token.exp) { return null; }

//   const date = new Date(0);
//   date.setUTCSeconds(token.exp);

//   return date;
// }

// function isTokenExpired(token) {
//   const expirationDate = getTokenExpirationDate(token);
//   return expirationDate < new Date();
// }