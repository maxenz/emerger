import axios from 'axios';

export function login(username, password){
         axios
            .get('http://localhost/Emerger.WebAPI/api/authentication/' + username + '/' + password)
            .then(function(response){
              console.log(response);
            });
}