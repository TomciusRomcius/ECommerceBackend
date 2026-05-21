import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadImages } from './upload-images';

describe('UploadImages', () => {
  let component: UploadImages;
  let fixture: ComponentFixture<UploadImages>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UploadImages]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UploadImages);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
