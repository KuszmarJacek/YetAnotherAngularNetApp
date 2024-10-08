import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FetchDataComponent } from './fetch-data.component';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { environment } from '../../environments/environment';

describe('FetcgDataComponent', () => {
  let component: FetchDataComponent;
  let fixture: ComponentFixture<FetchDataComponent>;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FetchDataComponent],
      imports: [HttpClientTestingModule]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FetchDataComponent);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should retrieve weather forecasts from the server', () => {
    const mockForecasts = [
      { date: '2021-10-01', temperatureC: 20, temperatureF: 68, summary: 'Mild' },
      { date: '2021-10-02', temperatureC: 25, temperatureF: 77, summary: 'Warm' }
    ];

    component.ngOnInit();

    const req = httpMock.expectOne(environment.baseUrl + 'api/weatherforecast');
    expect(req.request.method).toEqual('GET');
    req.flush(mockForecasts);

    expect(component.forecasts).toEqual(mockForecasts);
  });
});
