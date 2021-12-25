import { Inject, Injectable } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { BehaviorSubject } from 'rxjs';
import * as objectPath from 'object-path';
import { TranslationService } from 'src/app/modules/i18n/translation.service';
import { DefaultConfig } from '../../core/configs/default.config';
import { environment } from 'src/environments/environment';

const CONFIG_LOCAL_STORAGE_KEY = `${environment.appVersion}-layoutConfig`;

@Injectable({
  providedIn: 'root',
})
export class LayoutService {
  private configSubject: BehaviorSubject<any> = new BehaviorSubject<any>(undefined);

  constructor(@Inject(DOCUMENT) private document: Document, private translate: TranslationService) {}

  initConfig(): any {
    const configFromLocalStorage = localStorage.getItem(
      CONFIG_LOCAL_STORAGE_KEY
    );
    if (configFromLocalStorage) {
      try {
        this.configSubject.next(JSON.parse(configFromLocalStorage));
        return;
      } catch (error) {
        this.removeConfig();
        console.error('config parse from local storage', error);
      }
    }
    this.configSubject.next(DefaultConfig);
  }

  private removeConfig() {
    localStorage.removeItem(CONFIG_LOCAL_STORAGE_KEY);
  }

  refreshConfigToDefault() {
    this.setConfigWithPageRefresh(undefined);
  }

  getConfig(): any {
    const config = this.configSubject.value;
    if (!config) {
      return DefaultConfig;
    }
    return config;
  }

  setConfig(config: any) {
    if (!config) {
      this.removeConfig();
    } else {
      localStorage.setItem(
        CONFIG_LOCAL_STORAGE_KEY,
        JSON.stringify(config)
      );
    }
    this.configSubject.next(config);
  }

  setConfigWithoutLocalStorageChanges(config: any) {
    this.configSubject.next(config);
  }

  setConfigWithPageRefresh(config: any) {
    this.setConfig(config);
    document.location.reload();
  }

  getProp(path: string): any {
    return objectPath.get(this.configSubject.value, path);
  }

  setTheme() {
    const theme = this.getProp('theme.name');

    const head = this.document.getElementsByTagName('head')[0];
    let style = this.document.createElement('link');
    style.rel = 'stylesheet';
    if (theme) style.href = `${theme}.css`;
    else style.href = 'arya-blue.css'

    head.appendChild(style);
  }

  getThemeName() {
    return this.getProp('theme.name');
  }

  setDirection() {
    let dir = 'rtl'
    const lang = this.translate.getSelectedLanguage();
    if (lang === 'en') dir = 'ltr';

    const html = this.document.getElementsByTagName('html')[0];
    html.dir = dir;
    html.setAttribute('direction', dir)

    const head = this.document.getElementsByTagName('head')[0];
    const style = this.document.createElement('link');
    style.rel = 'stylesheet';
    style.href = `${dir}.css`;

    head.appendChild(style);
  }
}
