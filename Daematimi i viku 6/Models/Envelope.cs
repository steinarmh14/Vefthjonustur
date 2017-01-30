using System.Collections.Generic;

namespace CoursesAPI.Models
{
	/// <summary>
    /// This class contains information on how many pages are, how many courses are on a page, how many pages
	/// </summary>

    public class Envelope
    {
        /// <summary>
        ///  List of courses
        /// </summary>
        public List<CourseInstanceDTO> coursesList {get; set;}

        /// <summary>
        /// Contains info of a page
        /// </summary>
        public PageInfo pageInfo {get; set;}
        
    }
    /// <summary>
    /// The information of a page
    /// </summary>
    public class PageInfo
	{
        /// <summary>
        /// How many pages there are 
        /// example: Page count 2 (there are 2 pages)
        /// </summary>
        public int pageCount {get; set;}

        /// <summary>
        /// How many courses there are on a page
        /// example: Page size : 5 (there are 5 courses on a page)
        /// </summary>
        public int pageSize {get; set;}

        /// <summary>
        /// What page you want courses from
        /// example: Page number 1 (gets courses from the 1st page)
        /// </summary>
        public int pageNumber {get; set;}

        /// <summary>
        /// How many courses there are
        /// Example: total number of items: 10 (there are 10 courses)
        /// </summary>
        public int totalNumberOfItems {get; set;}
	}

}
