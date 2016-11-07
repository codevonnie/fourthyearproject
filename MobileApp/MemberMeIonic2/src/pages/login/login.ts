import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { TabsPage } from '../tabs/tabs';
import { Http, Headers } from '@angular/http';
import 'rxjs/add/operator/map';
import {Observable} from "rxjs";

/*
  Generated class for the Login page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-login',
  templateUrl: 'login.html'
})
export class LoginPage {

   accessToken: "YXBwNTc5NzU5MDAtYUVnQXRYOnRHbTZGd09LZ1U3c1F5UERVQUNq";

  constructor(public navCtrl: NavController, public http: Http) {}

  login(){
    let headers = new Headers();
    var URL = "http://localhost:8080/api/authenticate";
    var body = JSON.stringify({
    email: "spotts@email.com",
    password: "mypass"
    });
    headers.append('Authorization', 'Bearer ' + this.accessToken);
    headers.append('Content-Type', 'application/json');
 
    return this.http.post(URL, body, {headers: headers})
    .map(res => res.json());
 
  }

  goProfile(){
    this.navCtrl.setRoot(TabsPage);
  }

}
