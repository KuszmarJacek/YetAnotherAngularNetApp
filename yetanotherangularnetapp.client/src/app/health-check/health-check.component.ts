import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from '../../environments/environment.development';

interface Result {
  checks: Check[];
  totalStatus: string;
  totalResponseTime: number;
};

interface Check {
  name: string;
  responseTime: number;
  status: string;
  description: string;
};

@Component({
  selector: 'app-health-check',
  templateUrl: './health-check.component.html',
  styleUrl: './health-check.component.scss'
})
export class HealthCheckComponent implements OnInit {
  public result?: Result;
  constructor(private httpClient: HttpClient) { }

  ngOnInit() {
    this.httpClient.get<Result>(environment.baseUrl + 'api/health').subscribe({
      next: (result: any) => {
        this.result = result;
      },
      error: (err: any) => {
        console.error(err)
      }
    });
  }
}
