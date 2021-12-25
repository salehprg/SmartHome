import { SocketService } from './core/socket.service';
import { environment } from 'src/environments/environment';
import { HomeService } from 'src/app/modules/home/_services/home.service';
import { HttpClient } from '@angular/common/http';
import { NotifierService } from 'angular-notifier';
import { TranslateService } from '@ngx-translate/core';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { SplashScreenService } from './pages/splash-screen/splash-screen.service';
import { LayoutService } from './pages/_layout/layout.service';
import { TranslationService } from './modules/i18n/translation.service';
import { locale as enLang } from './modules/i18n/vocabs/en';
import { locale as faLang } from './modules/i18n/vocabs/fa';

@Component({
  selector: 'body[root]',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  private unsubscribe: Subscription[] = [];

  constructor(
    private splashScreenService: SplashScreenService,
    private router: Router,
    private layoutService: LayoutService,
    private translationService: TranslationService,
    private translate: TranslateService,
    private notifier: NotifierService,
    private http: HttpClient,
    private home: HomeService,
    private socket: SocketService // initilization
  ) {
    this.layoutService.initConfig();
    this.translationService.loadTranslations(enLang, faLang);
  }

  ngOnInit() {
    this.layoutService.setTheme();
    this.layoutService.setDirection();

    const routerSubscription = this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.splashScreenService.hide();
        window.scrollTo(0, 0);
        setTimeout(() => {
          document.body.classList.add('page-loaded')
        }, 500);
        this.notifier.hideAll();
      }
    });
    this.unsubscribe.push(routerSubscription);

    // this.socket.connection.invoke("TestData" , "This is Test Message")
    //   .catch(err => console.error(err));

    this.http.get(`${environment.apiUrl}/Auth`).subscribe(
      (res:any) =>{

        this.home._isRaspberry$.next(!environment.auth || res)
      },
      err =>{
      }
    )
  }

  ngOnDestroy() {
    this.unsubscribe.forEach((sb) => sb.unsubscribe());
  }
}
