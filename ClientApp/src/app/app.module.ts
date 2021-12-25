import { MusicModule } from './modules/music/music.module';
import { AlertComponent } from './pages/_layout/header/alert/alert.component';
import { DashboardModule } from './modules/dashboard/dashboard.module';
import { HomeModule } from './modules/home/home.module';
import { BarComponent } from './partials/general/bar/bar.component';
import { DevicesComponent } from './pages/_layout/header/devices/devices.component';
import { LanguageSelectorComponent } from './pages/_layout/header/language-selector/language-selector.component';
import { HeaderComponent } from './pages/_layout/header/header.component';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { InlineSVGModule } from 'ng-inline-svg';
import { AppRoutingModule } from './app-routing.module';
import { TranslationModule } from './modules/i18n/translation.module';

import { AppComponent } from './app.component';
import { LayoutComponent } from './pages/_layout/layout.component';
import { SplashScreenComponent } from './pages/splash-screen/splash-screen.component';
import { NgbDropdownModule, NgbModalModule, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ScenarioModule } from './modules/scenario/scenario.module';
import { GeneralModule } from './partials/general/general.module';
import { NotifierContainerComponent, NotifierModule } from 'angular-notifier';
import { WifiSelectorComponent } from './pages/_layout/header/wifi/wifi.component';
import { WifiCardComponent } from './pages/_layout/header/wifi/wifi-card/wifi-card.component';
import { WifiPasswordComponent } from './pages/_layout/wifi-password/wifi-password.component';
import { FormsModule } from '@angular/forms';
import { CameraModule } from './modules/camera/camera.module';
import { HttpClient } from 'selenium-webdriver/http';


@NgModule({
  declarations: [
    AppComponent,
    LayoutComponent,
    HeaderComponent,
    DevicesComponent,
    LanguageSelectorComponent,
    SplashScreenComponent,

    AlertComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    TranslateModule.forRoot(),
    InlineSVGModule.forRoot(),
    NgbModule,
    NgbDropdownModule,
    NgbModalModule,
    TranslationModule,
    AppRoutingModule,
    HomeModule,
    DashboardModule,
    CameraModule,
    ScenarioModule ,
    MusicModule,
    GeneralModule ,
    FormsModule,
    NotifierModule.withConfig({
      behaviour:{
        autoHide : 5000
      }
    })

  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
