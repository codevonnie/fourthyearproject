import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import {Http} from '@angular/http';
import { ProfileService } from '../../providers/profileservice';

@Component({
  selector: 'page-profile',
  templateUrl: 'profile.html',
  providers: [ProfileService]
})
export class ProfilePage {
  http: Http;
  profiles = [];

  constructor(public navCtrl: NavController, public profileservice: ProfileService) {
    this.loadProfile();
  }

  private loadProfile() {
    this.profileservice.getBusinessMembers()
    .subscribe(
      data => {
        this.profiles = data;

        //console.log(data);
        //console.log(JSON.parse(data));
        //console.log(JSON.stringify(this.profiles));
      },
      err => this.logError(err)
    );
  }

  public logError(err: TemplateStringsArray) {
    console.error('Error: ' + err);
  }

  ionViewDidLoad() {
    console.log('Hello Profile Page');
  }

}
