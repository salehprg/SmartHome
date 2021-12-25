import { environment } from './../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer } from '@angular/platform-browser';
import {
  ChangeDetectorRef,
  Component,
  HostListener,
  NgZone,
  OnInit,
} from '@angular/core';
import { AnimationOptions } from 'ngx-lottie';
import { registerLocaleData } from '@angular/common';
import localeFa from '@angular/common/locales/fa';
import { TranslateService } from '@ngx-translate/core';
import { interval } from 'rxjs';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'weather-widget',
  templateUrl: './weather-widget.component.html',
  styleUrls: ['./weather-widget.component.scss'],
})
export class WeatherWidget implements OnInit {
  now: Date;
  locale: any = 'en-EN';
  iconBaseUrl = './assets/media/animations/';
  weather: string;
  temp: number;
  hasError: boolean;
  bg: any;
  options: AnimationOptions = {};
  loading: boolean = false;
  public isNight: boolean;

  days: any[] = []

  city = 'Mashhad';

  constructor(
    private http: HttpClient,
    private sanitizer: DomSanitizer,
    private translate: TranslateService
  ) {
    registerLocaleData(localeFa);
    this.now = new Date();

    // translate.onLangChange.subscribe((lang) => {
    //   if (lang.lang === 'en') this.locale = 'en-US';
    //   else this.locale = 'fa-IR';
    // });
  }

  ngOnInit() {
    this.getWeather();

    const nums = interval(180 * 60 * 1000);

    nums.subscribe((x) => this.getWeather());

    this.getForecast()

    const forecast = interval(12 * 60 * 60 * 1000);

    forecast.subscribe((x) => this.getForecast())

    interval(10000).subscribe(() => {
      this.now = new Date();
    })
  }

  getDays(num: number) {
    let result = []
    switch (num) {
      case 1:
        result = [
          this.translate.instant('GENERAL.WEEK.TUESDAY'),
          this.translate.instant('GENERAL.WEEK.WEDNESDAY'),
          this.translate.instant('GENERAL.WEEK.THURSDAY')
        ]
        break;
      case 2:
        result = [
          this.translate.instant('GENERAL.WEEK.WEDNESDAY'),
          this.translate.instant('GENERAL.WEEK.THURSDAY'),
          this.translate.instant('GENERAL.WEEK.FRIDAY')
        ]
        break;
      case 3:
        result = [
          this.translate.instant('GENERAL.WEEK.THURSDAY'),
          this.translate.instant('GENERAL.WEEK.FRIDAY'),
          this.translate.instant('GENERAL.WEEK.SATURDAY')
        ]
        break;
      case 4:
        result = [
          this.translate.instant('GENERAL.WEEK.FRIDAY'),
          this.translate.instant('GENERAL.WEEK.SATURDAY'),
          this.translate.instant('GENERAL.WEEK.SUNDAY')
        ]
        break;
      case 5:
        result = [
          this.translate.instant('GENERAL.WEEK.SATURDAY'),
          this.translate.instant('GENERAL.WEEK.SUNDAY'),
          this.translate.instant('GENERAL.WEEK.MONDAY')
        ]
        break;
      case 6:
        result = [
          this.translate.instant('GENERAL.WEEK.SUNDAY'),
          this.translate.instant('GENERAL.WEEK.MONDAY'),
          this.translate.instant('GENERAL.WEEK.TUESDAY')
        ]
        break;
      case 0:
        result = [
          this.translate.instant('GENERAL.WEEK.MONDAY'),
          this.translate.instant('GENERAL.WEEK.TUESDAY'),
          this.translate.instant('GENERAL.WEEK.WEDNESDAY')
        ]
        break;
      default:
        break;
    }

    return result
  }

  reloadPage() {
    this.getWeather();
    this.getForecast();
  }

  getForecast() {
    this.http.get<any>(`${environment.apiUrl}/Wheater/forecast?day=3`).subscribe(
      (res: any) => {

        for (let i = 0; i < 3; i++) {
          this.days.push({
            date: this.getDays(new Date().getUTCDay())[i],
            temp: Math.floor(res[i].temp.day),
            desc: this.getWeatherSituation(res[i].weather[0].main)
          })
        }

      },
      err => {


      }
    )
  }

  getWeather() {
    this.loading = true;
    this.hasError = false;
    if (new Date().getHours() >= 17) {
      this.isNight = true;
    }

    this.http
      .get<any>(`${environment.apiUrl}/Wheater/city?cityName=Mashhad`)
      .subscribe(
        (res) => {
          this.weather = this.getWeatherSituation(res.weather[0].main);
          this.temp = Math.round(res.main.temp);
          this.setBG(this.getColor(res.weather[0].main));
          this.setAnimation(res.weather[0].main);

          this.loading = false;
          this.hasError = false;
        },
        (err) => {
          this.loading = false;
          this.hasError = true;
          const color = {
            c1: 'var(--surface-d)',
            c2: 'var(--surface-d)',
          };
          this.setBG(color);
        }
      );

    // this.weather = 'Snow'
    // this.temp = 22
    this.setBG(this.getColor(this.weather));
    this.setAnimation(this.weather);
  }

  getWeatherSituation(weather: string) {
    let result = '';
    switch (weather) {
      case 'Rain':
        result = this.translate.instant('WEATHER.STATUS.RAIN');
        break;
      case 'Drizzle':
        result = this.translate.instant('WEATHER.STATUS.DRIZZLE');
        break;
      case 'Snow':
        result = this.translate.instant('WEATHER.STATUS.SNOW');
        break;
      case 'Mist':
        result = this.translate.instant('WEATHER.STATUS.MIST');
        break;
      case 'Dust':
        result = this.translate.instant('WEATHER.STATUS.DUST');
        break;
      case 'Fog':
        result = this.translate.instant('WEATHER.STATUS.FOG');
        break;
      case 'Haze':
        result = this.translate.instant('WEATHER.STATUS.HAZE');
        break;
      case 'Smoke':
        result = this.translate.instant('WEATHER.STATUS.SMOKE');
        break;
      case 'Clear':
        result = this.translate.instant('WEATHER.STATUS.CLEAR');
        break;
      case 'Sand':
        result = this.translate.instant('WEATHER.STATUS.SAND');
        break;
      case 'Clouds':
        result = this.translate.instant('WEATHER.STATUS.CLOUDS');
        break;
      case 'Tornado':
        result = this.translate.instant('WEATHER.STATUS.TORNADO');
        break;
      case 'Squall':
        result = this.translate.instant('WEATHER.STATUS.SQUALL');
        break;
      case 'Ash':
        result = this.translate.instant('WEATHER.STATUS.ASH');
        break;
      case 'Thunderstorm':
        result = this.translate.instant('WEATHER.STATUS.THUNDERSTORM');
        break;
      default:
        break;
    }

    return result;
  }

  setAnimation(weather: string) {
    this.options = {
      ...this.options, // In case you have other properties that you want to copy
      path: this.getAnimation(weather),
    };
  }

  setBG(colors: any) {
    this.bg = this.sanitizer.bypassSecurityTrustStyle(
      `linear-gradient(to bottom right, ${colors.c1}, ${colors.c2})`
    );
  }

  getColor(weather: string) {
    let colors = {
      c1: 'var(--surface-d)',
      c2: 'var(--surface-d)',
    };

    if (!weather || weather.trim() === '') return colors;

    // const counter = interval(60 * 60 * 1000)

    // counter.subscribe(() => {
    const hour = new Date().getHours();

    if (hour >= 17) {
      switch (weather) {
        case 'Rain':
        case 'Drizzle':
          colors = { c1: '#16222A', c2: '#3A6073' };

          break;

        case 'Snow':
          colors = { c1: '#2b5876', c2: '#4e4376' };

          break;
        case 'Mist':
        case 'Dust':
          colors = { c1: '#24C6DC', c2: '#514A9D' };

          break;
        case 'Fog':
        case 'Haze':
        case 'Smoke':
          colors = { c1: '#134E5E', c2: '#71B280' };

          break;
        case 'Clear':
        case 'Sand':
          colors = { c1: '#614385', c2: '#516395' };

          break;
        case 'Clouds':
          colors = { c1: '#1F1C2C', c2: '#928DAB' };

          break;
        case 'Tornado':
        case 'Squall':
          colors = { c1: '#5433FF', c2: '#A5FECB' };

          break;
        case 'Ash':
        case 'Thunderstorm':
          colors = { c1: '#00416A', c2: '#E4E5E6' };

          break;
        default:
          break;
      }
    } else {
      switch (weather) {
        case 'Rain':
        case 'Drizzle':
          colors = { c1: '#00c3ff', c2: '#ffff1c' };

          break;

        case 'Snow':
          colors = { c1: '#4ecdc4', c2: '#556270' };

          break;
        case 'Mist':
        case 'Dust':
          colors = { c1: '#24C6DC', c2: '#514A9D' };

          break;
        case 'Fog':
        case 'Haze':
        case 'Smoke':
          colors = { c1: '#134E5E', c2: '#71B280' };

          break;
        case 'Clear':
        case 'Sand':
          colors = { c1: '#2F80ED', c2: '#56CCF2' };

          break;
        case 'Clouds':
          colors = { c1: '#00d2ff', c2: '#928dab' };

          break;
        case 'Tornado':
        case 'Squall':
          colors = { c1: '#5433FF', c2: '#A5FECB' };

          break;
        case 'Ash':
        case 'Thunderstorm':
          colors = { c1: '#00416A', c2: '#E4E5E6' };

          break;
        default:
          break;
      }
    }
    // })

    return colors;
  }

  getAnimation(weather: string) {
    let animation: string = '';

    // setInterval(() => {
    let hour = new Date().getHours();

    if (hour >= 17) {
      switch (weather) {
        case 'Rain':
        case 'Drizzle':
          animation = 'night-rainy';

          break;

        case 'Snow':
          animation = 'night-snowy';

          break;
        case 'Mist':
        case 'Dust':
          animation = 'misty';

          break;
        case 'Fog':
        case 'Haze':
        case 'Smoke':
          animation = 'foggy';

          break;
        case 'Clear':
        case 'Sand':
          animation = 'night';

          break;
        case 'Clouds':
          animation = 'night-cloudy';

          break;
        case 'Tornado':
        case 'Squall':
          animation = 'windy';

          break;
        case 'Ash':
        case 'Thunderstorm':
          animation = 'thunder';

          break;
        default:
          break;
      }
    } else {
      switch (weather) {
        case 'Rain':
        case 'Drizzle':
          animation = 'partly-rainy';

          break;

        case 'Snow':
          animation = 'snow-sunny';

          break;
        case 'Mist':
        case 'Dust':
          animation = 'misty';

          break;
        case 'Fog':
        case 'Haze':
        case 'Smoke':
          animation = 'foggy';

          break;
        case 'Clear':
        case 'Sand':
          animation = 'sunny';

          break;
        case 'Clouds':
          animation = 'partly-cloudy';

          break;
        case 'Tornado':
        case 'Squall':
          animation = 'windy';
          break;
        case 'Ash':
        case 'Thunderstorm':
          animation = 'thunder';
          break;
        default:
          break;
      }
    }

    return `${this.iconBaseUrl}/${animation}.json`;
  }
}
