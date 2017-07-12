import axios from 'axios';

const BASE_URL = 'http://localhost/Emerger.WebAPI';

export {login};

function login(username, password) {
  const url = `${BASE_URL}/api/authentication/${username}/${password}`;
  return axios.get(url).then(response => response.data);
}