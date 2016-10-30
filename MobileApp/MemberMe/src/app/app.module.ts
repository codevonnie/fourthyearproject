import { NgModule } from '@angular/core';
import { IonicApp, IonicModule } from 'ionic-angular';
import { MyApp } from './app.component';
import { LoginPage } from '../pages/login/login';
import { ProfilePage } from '../pages/profile/profile';
import { TabsPage } from '../pages/tabs/tabs';


@NgModule({
  declarations: [
    MyApp,
    LoginPage,
    ProfilePage,
    TabsPage
  ],
  imports: [
    IonicModule.forRoot(MyApp)
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp,
    LoginPage,
    ProfilePage,
    TabsPage
  ],
  providers: []
})
export class AppModule {}
