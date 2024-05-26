import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  primary: string;
  primaryLighter: string;
  warn: string
  warnOpacity: string;

  constructor() {
    // Récupérer les valeurs des variables CSS à partir du DOM
    this.primary = getComputedStyle(document.documentElement).getPropertyValue('--primary');
    this.primaryLighter = getComputedStyle(document.documentElement).getPropertyValue('--primary-lighter');
    this.warn = getComputedStyle(document.documentElement).getPropertyValue('--warn');
    this.warnOpacity = getComputedStyle(document.documentElement).getPropertyValue('--warn-opacity');
  }
    
  getPrimary(): string {
    return this.primary;
  }

  getPrimaryLighter(): string {
    return this.primaryLighter;
  }

  getWarn(): string {
    return this.warn;
  }

  getWarnOpacity(): string {
    return this.warnOpacity;
  }
}
